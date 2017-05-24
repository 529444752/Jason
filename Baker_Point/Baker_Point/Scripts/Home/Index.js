var a = new Array();
var c = new Array();
var i = 0;
var scroll = 0;
var IsFade = false;
var inter = self.setInterval(banner, 5000);


function NumSet() {
    var length = a.length - 1;
    if (i < length) {
        i++;
    }
    else
        i = 0;

}

$(window).scroll(function (event) {
    if ($(window).scrollTop() >= $(document).height - $(window).height) {
        alert("bottom");
    }
    if (!IsFade) {
        scroll = $(window).scrollTop();
    }
    else {
        $(window).scrollTop(scroll);
        IsFade = false;
    }
});
$(document).ready(function () {
    $.ajax({
        url: "/ProductId.xml",
        dataType: 'xml',
        type: 'GET',
        timeout: 2000,
        error: function (xml) {
            alert("加载XML 文件出错！");
        },
        success: function (xml) {
            var pro = $(xml).find("ProductID");
            $(xml).find("ProductID").each(function () {
                var oid = $(this).attr("ID");
                var name = $(this).attr("Name");
                a.push(oid);
                c.push(name);
            }
            );
            /*for (var j = 0; j < a.length / 2; j++) {
                var temp = a[j];
                a[j] = a[a.length - j - 1];
                a[a.length - 1 - j] = temp;
            }*/
            $("#ProductName").html(c[0]);
            $("#banner").attr("src", "/Images/Products/" + a[0] + ".jpg");
            $("#banner").click(function () {
                window.location = "../../Product/Details/" + a[0];
            });
        }
    });



    $("#next").click(function () {
        clearInterval(inter);
        var length = a.length - 1;
        if (i < length) {
            i++;
        }
        else
            i = 0;
        $("#ProductName").html(c[i]);
        $("#banner").attr("src", "/Images/Products/" + a[i] + ".jpg");
        inter = self.setInterval(banner, 5000);
    });

    $("#search_btn").click(function () {
        var text = $("#searchp").val();
        window.location = "../../Product/Index?CategoryId=0&productName=" + text;
    })

    $("#previous").click(function () {
        clearInterval(inter);
        if (i == 0) {
            i = a.length - 1;
        }
        else
            i--;
        $("#ProductName").html(c[i]);
        $("#banner").attr("src", "/Images/Products/" + a[i] + ".jpg");
        inter = self.setInterval(banner, 5000);
    });

    $(".ItemImg").each(function () {
        $(this).attr("src", $(this).attr("src") + ".jpg");
    });

    $(".LastViewImg").each(function () {
        $(this).attr("src", $(this).attr("src") + ".jpg");
    });

    $(".recommendImg").each(function () {
        $(this).attr("src", $(this).attr("src") + ".jpg");
    });

    var b = new Array();
    $(".ItemNum").each(function () {
        b.push($(this).text());
        $(this).html("x"+$(this).text());
    });
    i = 0;
    $(".ItemPrice").each(function () {
        $(this).html("￥" + parseFloat($(this).text()) * b[i]);
        i++;
    })

    i = 0;

    $("#bannerContainer").hover(function () {
        $("#productNameDiv").show(500);
    }, function () {
        $("#productNameDiv").hide(500);
    });

    $(".OrderItem").each(function () {
        $(this).animate({ opacity:1 }, 500);
    })
    $(".LastViewProduct").animate({ opacity: 1 }, 500, function () {
        
    });

    $(".recommendItem").each(function () {
        $(this).animate({ opacity: 1, marginTop: 0 }, 200);
    });

    if ($("#homeplist").css("left") == "650px")
    {
        $("#next").hide(0);
        $("#previous").hide(0);
        $("#banner").css("margin-top", "150px");
        $("#homeplist").css("opacity", "0.5");
        $("#homeplist").css("left", "800px");
        $("#banner").animate({ marginTop:0});
        $("#homeplist").animate({ opacity: 1, left: 650 }, function () { $("#next").show();$("#previous").show()});
    }
});


function banner() {
    IsFade = true;    
    $("#banner").fadeOut(function () {
        $("#banner").attr("src", "/Images/Products/" + a[i] + ".jpg");
        $("#ProductName").html(c[i]);
        IsFade = true;
        $("#banner").fadeIn();
        $("#banner").click(function () {
            window.location = "../../Product/Details/" + a[i];
        });
    });
    NumSet();

}

