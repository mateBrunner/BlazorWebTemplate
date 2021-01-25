
function GenerateSessionId(appName) {
    return appName + "__" + GetTimeString() + "__anonym__" + GenerateGUID();
}

function GetIp() {
    let ip;
    $.getJSON("https://api.ipify.org?format=json",
        function (data) {
            if (_g.Debug) console.log(ip);
            if (_g.Debug) console.log(data);
        })
    return ip;
}

function GetTimeString() {
    const d = (new Date()).toISOString();
    return d.replaceAll("-", "").replaceAll(":", "").replace("Z", "").replace(".", "");

}

function GetSegmentOfSessionId(sessionId, segmentIndex) {
    const splitted = sessionId.split("__");
    return splitted.length <= segmentIndex ? null : splitted[segmentIndex];
}

function ReplaceSegmentInSessionId(sessionId, segmentIndex, newValue) {
    let splitted = sessionId.split("__");
    if (splitted.length > segmentIndex)
        splitted[segmentIndex] = newValue;
    return splitted.join("__");
}

function GenerateGUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function Delay(t) {
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

const SessionIdSegments = {
    AppName: 0,
    InitDate: 1,
    Username: 2,
    GuidClient: 3,
    LoginDate: 4,
    GuidServer: 5
}