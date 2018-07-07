function TeamDet(sessionId) {
    this.sessionID = sessionId;
}

TeamDet.prototype.Load = function () {
    $("#btnCreateTeam").off("click");
    $("#btnCreateTeam").on("click", $.proxy(this.CreateTeamClicked, this));
}

TeamDet.prototype.CreateTeamClicked = function () {
    var name = $("#txtTeamName").val().trim();
    var teamId = $("#txtTeamId").val();
    var journeyId = $("#txtJourneyId").val();
    var memberId = this.sessionID;
 
    if (name == '') { alert('please enter TeamName'); $("#txtTeamName").focus(); }
    else if (memberId == undefined || memberId == '') { alert('Session has been expired!'); window.location.href = '../Journey/Index'; }
    else {
        var model = { TeamId: teamId, Name: name, JourneyId: journeyId, MemberId: memberId };
        $.ajax({
            url: "/api/team",
            type: 'post',
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.status == "OK") {
                    alert("New team is saved Successfully..!");
                    if (teamId == undefined || teamId == '') InviteTeamLeader(memberId, result.data.teamJourneyId);
                    else window.location.href = '../Journey/Index';
                }
                else {
                    alert("team creation is failed..!");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("team creation is failed..!");
            }
        });
    }
}

function InviteTeamLeader(memberId, teamJourneyID) {
    var model = { TeamJourneyId: teamJourneyID, MemberId: memberId, Role: 1 }
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

function myFunction() {
    var formData = new FormData();
    var file = document.getElementById("file");
    formData.append("file", file);

    $.ajax({
        url: '/api/teamAvatar',
        method: 'post',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        cache: false,
        success: function (result) {
            if (result.status == "success") {
            }
            else {
                alert("An error occurred during the save process.");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("An error occurred during the save process.");
        }
    });
}
