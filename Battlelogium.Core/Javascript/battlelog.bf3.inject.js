﻿/// <reference path="windowbutton/battlelog.windowbutton.js" />
/// <reference path="playbar/battlelog.bf3.playbar.js" />
/// <reference path="dialog/battlelog.bf3.dialog.js" />
/// <reference path="stats/battlelog.bf3.stats.js" />
var baseurl = 'http://localhost/battlelogium';
function injectOnce() {
    if (document.getElementById('_windowbutton') == null) {
        injectScript('_windowbutton', baseurl+'/windowbutton/battlelog.windowbutton.min.js');
    }
    if (document.getElementById('css_windowbutton') == null) {
        injectCSS('css_windowbutton', baseurl + '/windowbutton/battlelog.windowbutton.min.css');
    }
    if (document.getElementById('_battlelogplaybar') == null) {
        injectScript('_battlelogplaybar', baseurl + '/playbar/battlelog.bf3.playbar.min.js');
    }
    if (document.getElementById('_battlelogdialog') == null) {
        injectScript('_battlelogdialog', baseurl + '/dialog/battlelog.bf3.dialog.min.js');
    }
    if (document.getElementById('_battlelogstats') == null) {
        injectScript('_battlelogstats', baseurl + '/stats/battlelog.bf3.stats.min.js');
    }
    if (document.getElementById('_battlelogurlchange') == null) {
        injectScript('_battlelogurlchange', baseurl + '/battlelog.bf4.urlchange.min.js');
    }
}

function runCustomJS() {
    windowbutton.addChromeButtons();
    battlelogplaybar.fixEAPlaybarButtons();
    battlelogplaybar.fixQuickMatchButtons();
    battlelogplaybar.addPlaybarButton(battlelogplaybar.createPlaybarButton('btnServers', 'SERVERS', 'location.href = "http://battlelog.battlefield.com/bf3/servers/"'));
    $("#base-header-secondary-nav>ul>li>a:contains('Buy Battlefield 4')").remove();
}

function runOnURLChange() {
    if (window.location.href.match(/\/soldier\//) != null) {
        battlelogstats.overview();
    }
}

function injectScript(id, url) {
    var script = document.createElement('script');
    script.setAttribute('src', url);
    script.setAttribute('id', id);
    document.getElementsByTagName('head')[0].appendChild(script);
}
function injectCSS(id, url) {
    var script = document.createElement('link');
    script.setAttribute('rel', 'stylesheet');
    script.setAttribute('type', 'text/css');
    script.setAttribute('href', url);
    script.setAttribute('id', id);
    document.getElementsByTagName('head')[0].appendChild(script);
}

injectOnce();