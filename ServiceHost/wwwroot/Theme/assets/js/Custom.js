async function SetDefaultAdderss(id) {
    const fullUrl = location.protocol + '//' + location.host;
    var headersList = {
        "Content-Type": "application/json"
    }
    var data = { Id: id };
    var json = JSON.stringify(data);
    var response = await fetch(fullUrl + "/api/Delivery/SetDefaultAddress", {
        method: "POST",
        body: json,
        headers: headersList
    });
    response.text().then(function (text) {

        var result = JSON.parse(text);

    });
}