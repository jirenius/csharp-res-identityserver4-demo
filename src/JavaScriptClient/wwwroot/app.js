/// <reference path="oidc-client.js" />

function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", api, false);
document.getElementById("logout").addEventListener("click", logout, false);

var config = {
    authority: "http://localhost:5000",
    client_id: "js",
    redirect_uri: "http://localhost:5003/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri : "http://localhost:5003/index.html",
};
var mgr = new Oidc.UserManager(config);

var ResClient = resclient.default;
var client = new ResClient('ws://localhost:8080');
client.setOnConnect(() => mgr.getUser().then(function (user) {
    if (user) {
        return client.authenticate('authentication.user', 'jwt', { token: user.access_token })
            .catch(err => {
                log("Failed to authenticate: " + err.message);
            });
    }
}));

mgr.getUser().then(function (user) {
    if (user) {
        log("User logged in", user.profile);
    }
    else {
        log("User not logged in");
    }
});

function login() {
    mgr.signinRedirect();
}

function api() {
    client.get('awesomeTicker.ticker').then(ticker => {
        log(ticker);
        ticker.on('change', () => {
            log(ticker);
        });
    }).catch(err => {
        log("Failed to get ticker: " + err.message);
    });
}

function logout() {
    mgr.signoutRedirect();
}