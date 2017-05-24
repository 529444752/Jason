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
            $(".mpic").each(function () {
                $(this).attr("src", "/Images/Products/" + a[i] + ".jpg");
                i++;
            });
            i = 0;
            $(".add").each(function () {
                var id = a[i];
                $(this).click(function () {
                    window.location.href("../../Product/Cart/" + id + "?mode=1");
                    window.location.reload();
                    
                });
                i++;
                
            })
            i = 0;
            $(".decrease").each(function () {
                var id = a[i];
                $(this).click(function () {
                    window.location.href("../../Product/Cart/" + id + "?mode=0");
                    window.location.reload();
                });
                i++;
            })
            i = 0;
            $(".num").each(function () {
                $(this).html(b[i]);
                i++;
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