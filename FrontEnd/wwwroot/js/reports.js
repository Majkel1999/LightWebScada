async function getReport(id, body) {
    let res = await fetch("http://maluch.mikr.us:30104/report?OrganizationId=" + id, {
        method: "POST",
        body: body,
        headers: {
            'accept': '*/*',
            'Content-Type': 'application/json'
        }
    }).catch(err => {
        console.log(err);
        return;
    });
    let blob = await res.blob();
    let bloburl = window.URL.createObjectURL(blob);
    let a = document.createElement('a');
    a.href = bloburl;
    a.download = 'Report.html';
    a.click();
}

