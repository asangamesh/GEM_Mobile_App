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
            async: false,
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
    else if (validateForm(email)) alert("Not valid email address");
    else {
        $.ajax({
            url: "../users/Logon",
            type: 'get',
            cache: false,
            async: false,
            data: { 'email': email },
            success: function (result) {
                if (result.RoleAccess == 1) window.location.href = '../Journey/Create';
                else window.location.href = '../Team/Index';
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("You have entered incorrect username and password");
            }
        });
    }
}

function validateForm(x) {
    var atpos = x.indexOf("@");
    var dotpos = x.lastIndexOf(".");
    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) return true;
    else return false;
}