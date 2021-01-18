var importedModules = {};

handleLogin = async function (username, modules, time) {
    window.name = generateSessionId(username, time);

    for (let i = 0; i < modules.length; i++) {
        var filename = './' + modules[i] + '.js';
        try {
            console.log("import " + filename);
            importedModules[modules[i]] = await import(filename);
        } catch (error) {
            console.log(error);
        }
    };

    return window.name;
}

checkSessionId = function (username, time) {
    if (window.name === "") {
        if (username != null)
            window.name = generateSessionId(username, time);
    }
    return window.name;
}

generateSessionId = function (username, time) {
    return username + "_" + time;
}

handleSuccessfulLogin = async function () {
    importedModules.authenticated = await import('./authenticated.js')
}

callModuleFunction = async function (moduleName, functionName, param) {
    console.log(moduleName + ' ' + functionName + ' ' + param)
    console.log(importedModules);
    //if (window["importedModules"].hasOwnProperty(moduleName) && window["importedModules"][moduleName].hasOwnProperty(functionName))
        window["importedModules"][moduleName][functionName](param);
    //else 
        //console.log("function is not accessible")
}
