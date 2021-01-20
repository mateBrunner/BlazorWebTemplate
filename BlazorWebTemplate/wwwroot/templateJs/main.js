var importedModules = {};
var otherWindows = [];
const channel = new BroadcastChannel('geo-channel');


initClient = async function (time) {

    if (window.name != "") {
        clientData = {
            sessionId: window.name,
            ip: "ipnumber"
        }

        return JSON.stringify(clientData);
    }

    await delay(600);

    var sessionId;
    if (otherWindows.length > 0) {
        sessionId = otherWindows[0];
        sessionId = sessionId.replace(sessionId.split("_")[0], time);
    }        
    else {
        sessionId = generateSessionId("anonym", time)
    }

    window.name = sessionId;
    startBroadcasting();
  
}

handleFirstLogin = async function (sessionId, modules) {
    var data = {
        sessionId: sessionId,
        modules: modules
    }

    var message = new Message(MessageTypes.Login, sessionId, data)
    channel.postMessage(message);

    handleLogin(data);
}

handleLogin = async function (data) {
    var initTime = window.name.split("_")[0]
    var newSessionId = initTime + data.sessionId.substr(initTime.length)
    console.log(newSessionId);
    window.name = data.sessionId;

    for (let i = 0; i < data.modules.length; i++) {
        var filename = './' + data.modules[i] + '.js';
        try {
            console.log("import " + filename);
            importedModules[data.modules[i]] = await import(filename);
            //return window.name;
        } catch (error) {
            console.log(error);
        }
    };
    console.log("handleLogin lefutott")
    console.log(data)

}

getClientData = async function (username, time) {
    if (window.name == "") {
        await initClient(time)
    }


    clientData = {
        sessionId: window.name,
        ip: "ipnumber"
    }

    return JSON.stringify(clientData);

    return window.name;
}

generateSessionId = function (username, time) {
    return time + "_" + username + "_" + generateGUID();  
}

handleSuccessfulLogin = async function () {
    importedModules.authenticated = await import('./authenticated.js')
}

callModuleFunction = async function (moduleName, functionName, param) {
    console.log(moduleName + ' ' + functionName + ' ' + param)
    console.log(importedModules);
    if (window["importedModules"].hasOwnProperty(moduleName) && window["importedModules"][moduleName].hasOwnProperty(functionName))
        window["importedModules"][moduleName][functionName](param);
    else 
        console.log("function is not accessible")
}



startBroadcasting = async function() {
    console.log("start broadcast")
    while (true) {
        await delay(500);
        var message = new Message(MessageTypes.Refresh, window.name, window.name)
        channel.postMessage(message);
    }
}

channel.onmessage = function (message) {
    if (message.data.type == MessageTypes.Refresh) {
        if (!otherWindows.includes(message.data.sender)) {
            otherWindows.push(message.data.sender);
            otherWindows.sort();
        }
    } else if (message.data.type === MessageTypes.CloseWindow) {
        otherWindows.pop(message.data.sender)
    } else if (message.data.type === MessageTypes.Login) {
        handleLogin(message.data.message);
    }


}

function getIp() {
    var ip;
    $.getJSON("https://api.ipify.org?format=json",
        function (data) {
            console.log(ip);
        })
    return ip;
}

function generateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function delay(t) {
    return new Promise(resolve => setTimeout(resolve, t));
}


function Message(type, sender, message, control) {
    this.type = type;
    this.sender = sender;
    this.message = message;
    this.control = control;
}

var MessageTypes = {
    Refresh: "refresh",
    CloseWindow: "closeWindow",
    Login: "login"
}



window.onbeforeunload = function () {
    var message = new Message(MessageTypes.CloseWindow, window.name, window.name)
    channel.postMessage(message);
}