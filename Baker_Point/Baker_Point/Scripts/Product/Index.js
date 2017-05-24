
function StandardTaxRate() {
    $.ajax({
        url: "/Scripts/Product/ProductId.xml",
        dataType: 'xml',
        type: 'GET',
        timeout: 2000,
        error: function (xml) {
            alert("加载XML 文件出错！");
        },
        success: function (xml) {
            var i = 0;
            var a = new Array();
            $(xml).find("ProductID").each(function () {
                var oid = $(this).attr("ID");
                a.push(oid);
            });
            $(".pic").each(function () {
                $(this).attr("src", "/Images/Products/" + a[i] + ".jpg");
                i++;
            })
        }
    });
}