function Journey(sessionId) {
    this.sessionID = sessionId;
}

Journey.prototype.Load = function () {
    $("#btnInvite").off("click");
    $("#btnInvite").on("click", $.proxy(this.InviteClicked, this));

    $(".btnStartJourney").off("click");
    $(".btnStartJourney").on("click", $.proxy(this.SelectJourneyClicked, this));
}

Journey.prototype.InviteClicked = function () {
    var memberId = this.sessionID;
    var teamJourneyId = $('#Team_Journey_Id').val();
    var email = $("#mail-invitation").val();
    if (email == '') { alert("please enter email address"); $("#mail-invitation").focus(); }
    else if (validateForm(email)) alert("Not valid email address");
    else if (memberId == undefined || memberId == '') { alert('Session has been expired!'); window.location.href = '../Journey/Index'; }
    else {
        var model = { TeamJourneyId: teamJourneyId, MemberId: memberId, EmailAddress: email, Role: 2 }
        $.ajax({
            url: "/api/teamMember",
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

Journey.prototype.SelectJourneyClicked = function () {
    var memberId = this.sessionID;
    var teamJourneyId = $('#Team_Journey_Id').val();
    if (teamJourneyId == undefined || teamJourneyId == '') { alert("please select team first.."); }
    else if (memberId == undefined || memberId == '') { alert('Session has been expired!'); window.location.href = '../Journey/Index'; }
    else {
        $.ajax({
            url: "../Journey/CreateMission",
            type: 'get',
            cache: false,
            data: { teamJourneyId: teamJourneyId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.success == true) {
                    window.location.href = '../Mission/Index?teamJourneyId=' + teamJourneyId;
                }
                else {
                    alert(result.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
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