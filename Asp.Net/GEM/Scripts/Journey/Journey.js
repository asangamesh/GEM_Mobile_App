function Journey(sessionId) {
    this.sessionID = sessionId;
}

Journey.prototype.Load = function () {
    $("#btnInvite").off("click");
    $("#btnInvite").on("click", $.proxy(this.InviteClicked, this));
}

Journey.prototype.InviteClicked = function () {
    var memberId = this.sessionID;
    var teamJourneyId = $('#Team_Journey_Id').val();
    var email = $("#mail-invitation").val();
    if (email == '') { alert("please enter email address"); $("#mail-invitation").focus(); }
    else if (memberId == undefined || memberId == '') { alert('Session has been expired!'); window.location.href = '../Journey/Index'; }
    else {
        var model = { TeamJourneyId: teamJourneyId, MemberId: memberId, EmailAddress: email, Role: 2 }
        $.ajax({
            url: "/api/journey",
            type: 'post',
            cache: false,
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data.status == true) {
                    window.location.href = '../Journey/Index';
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
