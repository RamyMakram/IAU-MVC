var language = GetCookie(); 

function GetCookie() {
    let name = "lang=";
    let cookieValue = null;
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            cookieValue = c.substring(name.length, c.length);
        }
    }
    return cookieValue;
}

console.log(language);