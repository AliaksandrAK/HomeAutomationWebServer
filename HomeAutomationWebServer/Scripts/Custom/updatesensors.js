function updateCpuInfo( )
{
    var lPathName = window.location.pathname.toLowerCase();
    var baseUrl2 = window.location.origin + "/Home/UpdateCpuInfo";

    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        url: baseUrl2,
        success: function (data) {

            var cpuSummary = jQuery.parseJSON(data.jsonPR);
            if (cpuSummary != null) {
                var ff = $("#power-id");
                $("#power-id").val(cpuSummary.Power);
                $("#load-id").val(cpuSummary.Load);
            }
        }
    });
}
