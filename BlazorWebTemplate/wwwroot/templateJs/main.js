"use strict" 
const _g = {};
_g.AppName = "BlazorWebTemplate"
_g.Debug = true;
_g.ImportedModules = {};
_g.AllTabs = [];
_g.Channel = new BroadcastChannel('geo-channel');
_g.SyncMessage;


async function AppInit(serverGuid, loginTime) {

    if (window.name != "") {
        StartBroadcasting();
        return;
    }

   
    //ha custom authentikáció van
    if (serverGuid == null) {
        var sessionId = GenerateSessionId(_g.AppName);
        InitWindowName(sessionId);
        
    } else {
        //ha windows-os authentikáció van

        var sessionId = GenerateSessionId(_g.AppName, serverGuid, loginTime);

        //TODO - itt elég lenne egy stringet küldeni
        const data = { oldSessionId: sessionId }

        $.ajax({
            url: "template/windowsUser",
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf8",
            success: function (newSessionId) {

                InitWindowName(newSessionId);

            }
        });

    }

}

function InitWindowName(sessionId) {
    window.name = sessionId;
    _g.AllTabs.push(sessionId);
    if (_g.Debug) console.log("init window name: " + window.name)
    StartBroadcasting();
}

async function GetClientData() {

    if (window.name == "")
        return null;
        //AppInit();
    

    const clientData = {
        sessionId: window.name,
        ip: "ipnumber"
    }

    return JSON.stringify(clientData);

    return window.name;
}


async function CallModuleFunction(moduleName, functionName, param) {
    if (window["_g"]["ImportedModules"].hasOwnProperty(moduleName) &&
        window["_g"]["ImportedModules"][moduleName].hasOwnProperty(functionName))
        window["_g"]["ImportedModules"][moduleName][functionName](param);
    else 
        if (_g.Debug) console.log("function is not accessible")
}

async function StartBroadcasting() {
    if (_g.Debug) console.log("start broadcast")
    _g.SyncMessage = new Message(MessageTypes.Refresh, window.name, window.name)
    while (true) {
        await Delay(500);
        _g.Channel.postMessage(_g.SyncMessage);
    }
}

_g.Channel.onmessage = function (message) {
    if (window.name == "")
        return;
    if (message.data.type == MessageTypes.Refresh) {
        if (!_g.AllTabs.includes(message.data.sender)) {
            HandleNewWindow(message.data.sender);
        }
    } else if (message.data.type == MessageTypes.CloseWindow) {
        const index = _g.AllTabs.pop(message.data.sender);
        _g.AllTabs.splice(index, 1);
    }
}

function HandleNewWindow(otherSessionId) {
    const isSameGuidClient = GetSegmentOfSessionId(otherSessionId, SessionIdSegments.GuidClient) ==
                             GetSegmentOfSessionId(window.name, SessionIdSegments.GuidClient);
    const hasOtherLoggedIn = GetSegmentOfSessionId(otherSessionId, SessionIdSegments.GuidServer) != null;
    const hasThisLoggedIn = GetSegmentOfSessionId(window.name, SessionIdSegments.GuidServer) != null;
    const isLoginNeeded = hasOtherLoggedIn && !hasThisLoggedIn;
    const isOtherOlder = GetSegmentOfSessionId(otherSessionId, SessionIdSegments.InitDate) <
                         GetSegmentOfSessionId(window.name, SessionIdSegments.InitDate);



    if ((isLoginNeeded && !isSameGuidClient && !isOtherOlder) || (!isLoginNeeded && (isSameGuidClient || !isOtherOlder))) {
        InsertTab(otherSessionId, true);
        return;
    }

    console.log("isSameGuidClient " + isSameGuidClient);
    console.log("hasOtherLoggedIn " + hasOtherLoggedIn);
    console.log("hasThisLoggedIn " + hasThisLoggedIn);
    console.log("isLoginNeeded " + isLoginNeeded);
    console.log("isOtherOlder " + isOtherOlder);

    const newSessionId = ReplaceSegmentInSessionId(
        otherSessionId,
        SessionIdSegments.InitDate,
        GetSegmentOfSessionId(window.name, SessionIdSegments.InitDate))

    const data = {
        senderSessionId: otherSessionId,
        oldSessionId: window.name,
        newSessionId: newSessionId,
        isLoginNeeded: isLoginNeeded
    }

    console.log(data);

    $.ajax({
        url: "template/changeSessionId",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf8",
        success: function (data2) {

            InsertTab(otherSessionId);

            SetNewSessionId(newSessionId);

        }
    });

}

function InsertTab(sessionId) {
    if (_g.AllTabs.includes(sessionId))
        return;

    const date = GetSegmentOfSessionId(sessionId, SessionIdSegments.InitDate);
    let justModify = false;
    let insertIndex = 0;
    for (let i = 0; i < _g.AllTabs.length; i++) {
        let date_i = GetSegmentOfSessionId(_g.AllTabs[i], SessionIdSegments.InitDate);

        if (date_i == date) {
            justModify = true;
            break;
        } else if (date_i < date) 
            insertIndex++;
        else
            break;
    }
    if (justModify)
        _g.AllTabs[insertIndex] = sessionId;
    else
        _g.AllTabs.splice(insertIndex, 0, sessionId);


    if (_g.Debug) console.log(_g.AllTabs);
}

function IsLoginNeeded(otherSessionId) {
    return GetSegmentOfSessionId(otherSessionId, SessionIdSegments.GuidServer) != null &&
        GetSegmentOfSessionId(window.name, SessionIdSegments.GuidServer == null)
}

async function HandleLogin(newSessionId, modules) {
    SetNewSessionId(newSessionId);

    for (let i = 0; i < modules.length; i++) {
        var filename = './' + modules[i] + '.js';
        try {
            if (_g.Debug) console.log("import " + filename);
            _g.ImportedModules[modules[i]] = await import(filename);
            //return window.name;
        } catch (error) {
            if (_g.Debug) console.log(error);
        }
    };
}

function SetNewSessionId(newSessionId) {
    // ez még nem biztos, hogy jó így
    InsertTab(newSessionId);
    _g.SyncMessage.sender = newSessionId;
    _g.SyncMessage.message = newSessionId;
    window.name = newSessionId;
    
    console.log("myNewSessionId: " + newSessionId)
}




window.onbeforeunload = function () {
    let message = new Message(MessageTypes.CloseWindow, window.name, window.name)
    _g.Channel.postMessage(message);
}