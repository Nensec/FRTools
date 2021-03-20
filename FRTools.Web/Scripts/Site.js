copyOrGenerateUrl = (e, b, t, id) => {
    if (e.value != '') {
        e.select();
        document.execCommand('copy');
        b.innerHTML = 'Copied!';
    }
    else {
        b.innerHTML = 'Fetching..';
        $.get({
            url: '@Url.RouteUrl("GetShareUrl")',
            data: { type: t, id: id },
            success: d => { e.value = d; copyText(e, b); }
        }).fail(() => b.innerHTML = 'Error!');
    }
}

copyText = (e, b) => {
    e.select();
    document.execCommand('copy');
    b.innerHTML = 'Copied!';
}