var templateData = {};


successfulLogin = async function(message) {

    templateData.js = await import('./authenticated.js')
    console.log("import authenticated.js")

}