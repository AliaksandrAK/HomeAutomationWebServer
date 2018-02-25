function updateCompInfo()
{
    var lPathName = window.location.pathname.toLowerCase();
    var baseUrl2 = window.location.origin + "/Home/UpdateCompInfo";
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        contentType: 'application/json; charset=utf-8',
        url: baseUrl2,
        success: function (data) {

            var cpuSummary = jQuery.parseJSON(data.jsonPR);
            if (cpuSummary != null) {
                $("#power-id").val(cpuSummary.CpuInfo.Power);
                $("#load-id").val(cpuSummary.CpuInfo.Load);
                $.each(cpuSummary.CpuInfo.items, function (i, val) {
                    var id = "#cpu-input-item" + i;
                    $(id).val(val.Temperature);
                });
                $.each(cpuSummary.VideoInfo, function (i, val) {
                    var id = "#video-input-item" + i;
                    $(id).val(val.Temperature);
                });
            }
        }
    });
}
