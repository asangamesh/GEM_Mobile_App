function User() {

}

User.prototype.Load = function () {
    $("#btnLogin").off("click");
    $("#btnLogin").on("click", $.proxy(this.LoginClicked, this));

    $("#btnCreate").off("click");
    $("#btnCreate").on("click", $.proxy(this.CreateClicked, this));
}

User.prototype.CreateClicked = function () {

    var email = $("#txtEmail").val();
    if (email == '') { alert("please enter email address"); $("#txtEmail").focus(); }
    else {
        var model = { EmailAddress: email }
        $.ajax({
            url: "/api/user",
            type: 'post',
            cache: false,
            data: JSON.stringify( model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data.status == true) {
                    SetValue(result.data.userId);
                    window.location.href = '../Users/Welcome';
                }
                else {
                    alert(result.data.message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
}

User.prototype.LoginClicked = function () {

    var email = $("#txtEmail").val();

    if (email == '') { alert('please enter email address'); $("#txtEmail").focus(); }
    else {
        $.ajax({
            url: "/api/user",
            type: 'get',
            cache: false,
            async: false,
            data: { 'email': email },
            success: function (result) {
                if (result.data.status == true) {
                    SetValue(result.data.user.userId);
                    window.location.href = '../Journey/Create';
                }
                else {
                    alert(result.data.message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("You have entered incorrect username and password");
            }
        });
    }
}

function SetValue(myval) {
    $.ajax({
        url: "../Users/SetSession",
        Type: 'POST',
        data: { sessionval: myval },
        success: function (result) {
        },
    });
}
