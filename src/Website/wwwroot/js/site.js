
function solrCallback(response) {
    post('/UploadSolrRaw', { raw: JSON.stringify(response) });
}

async function fetchUrlContent(url) {
    try {
        $.getScript(url);
    }
    catch (err) {
        console.log('fetch failed', err);
    }

    // If we get this far, there was an issue, so lets show the modal
    if (!isSecure()) {
        $('#mixedContentModal').modal('show')
    }
}

function getUrl() {
    var url = new URL($("#SolrUrl").val());

    var query_string = url.search;

    var urlParams = new URLSearchParams(query_string);

    // &wt=json&indent=true&debugQuery=true

    // wt
    if (urlParams.has("wt")) {
        urlParams.set('wt', 'json');
    }
    else {
        urlParams.append('wt', 'json');
    }

    // indent
    if (urlParams.has("indent")) {
        urlParams.set('indent', 'true');
    }
    else {
        urlParams.append('indent', 'true');
    }

    // debugQuery
    if (urlParams.has("debugQuery")) {
        urlParams.set('debugQuery', 'true');
    }
    else {
        urlParams.append('debugQuery', 'true');
    }

    urlParams.append("json.wrf","solrCallback");

    // change the search property of the main url
    url.search = urlParams.toString();

    // the new url string
    var new_url = url.toString();

    console.log(new_url);

    return new_url.toString();
}

function isSecure() {
    var url = new URL($("#SolrUrl").val());
    if (url.protocol == "https:") {
        return true;
    }
    else {
        return false;
    }
}

function post(path, params, method) {
    method = method || "post"; // Set method to post by default if not specified.

    // The rest of this code assumes you are not using a library.
    // It can be made less wordy if you use one.
    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
}

function showMixedContentModal() {

}

$(document).ready(function () {
    $("#LoadFromSolr").click(function () {
        var url = getUrl();
        fetchUrlContent(url);
    });

    $("#PostFromTextArea").click(function () {
        var data = document.getElementById("SolrTextArea").value;

        post('/UploadSolrRaw', { raw: data });
    });
});