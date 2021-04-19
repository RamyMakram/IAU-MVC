// Language JSON File Location
var language = localStorage.getItem('language');
// Default Language
var default_lang = 'en';
var HomeUrl = window.location.protocol + "//" + window.location.hostname + (window.location.port != "" ? ":" + window.location.port : "");

// Set Selected Language
function setLanguage(lang) {
    if(lang=='en') {
        document.getElementById("header-lang-img").src = HomeUrl+"/assets/images/flags/us.jpg";
    } else if(lang=='ar') {
        document.getElementById("header-lang-img").src = HomeUrl +"/assets/images/flags/sw.jpg";
    }
    localStorage.setItem('language', lang);
    language = localStorage.getItem('language');
    // Run Multi Language Plugin
    getLanguage()
}

// Run Multi Language Plugin
function getLanguage() {
    // Language on user preference
    (language == null) ? setLanguage(default_lang) : false;
    // Load data of selected language
    $.ajax({
        url: HomeUrl +'assets/lang/' + language + '.json',
        dataType: 'json', async: true
    }).done(function (lang) {
        // add selected language class to the body tag
        $('html').attr('lang', language);
        // Loop through message in data
        $.each(lang, function (index, val) {
            (index === 'head') ? $(document).attr("title", val['title']) : false;
            $(index).children().each(function () {
                $(this).text(val[$(this).attr('key')])
            })
            $(index).children().children().each(function () {
                $(this).text(val[$(this).attr('key')])
            })
        })
    })
}

// Auto Loader
$(document).ready(function () {
    if (language != null && language !== default_lang)
        getLanguage(language);
});
