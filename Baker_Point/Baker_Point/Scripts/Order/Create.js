function StandardTaxRate() {
    $.ajax({
        url: "/Mart.xml",
        dataType: 'xml',
        type: 'GET',
        timeout: 2000,
        error: function (xml) {
            alert("加载XML 文件出错！");
        },
        success: function (xml) {
            var i = 0;
            var a = new Array();
            var b = new Array();
            $(xml).find("ProductID").each(function () {
                var oid = $(this).attr("ID");
                var num = $(this).attr("Num");
                a.push(oid);
                b.push(num);
            });
            i = 0;
            var total = 0;
            $(".price").each(function () {
                total+=parseFloat($(this).text())*b[i];
                $(this).html("￥"+parseFloat($(this).text())*b[i]);
                i++;
            });
            $("#total").html("Total:￥"+total);

        }
    });
}