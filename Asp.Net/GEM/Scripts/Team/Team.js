function TeamDet(sessionId) {
    this.sessionID = sessionId;
}

TeamDet.prototype.Load = function () {
    $("#btnCreateTeam").off("click");
    $("#btnCreateTeam").on("click", $.proxy(this.CreateTeamClicked, this));

    $(document).ready(function () {
        $('#accept-practice-btn').click(function () {
            $('#myMemberModal').modal('hide');
        });
        $('#reject-practice-btn').click(function () {
            $('#myMemberModal').modal('hide');
            $('#rejectionModal').modal('show');
        });
    });
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
            async: true,
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data.status == true) {
                    jQuery('#teamModal').modal('hide');
                    //jQuery('#congratsModelBox').modal('show');
                    myFunction(result.data.teamId);
                    if (teamId == undefined || teamId == '') InviteTeamLeader(memberId, result.data.teamJourneyId);
                    else window.location.href = '../Journey/Index';
                }
                else {
                    alert(result.data.message);
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
        url: "/api/teamMember",
        type: 'post',
        cache: false,
        async:true,
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

function myFunction(teamId) {
    if (window.FormData != undefined) {

        var fileUpload = $("#imgInp").get(0);
        var files = fileUpload.files;
        var newImageName = "team" + teamId;
        var fileData = new FormData();

        fileData.append(teamId, files[0]);
        $.ajax({
            url: '/Journey/UploadFiles?ImageName=' + newImageName + '',
            type: "POST",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result)
            {
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    } 
}

function vote(id, thumbs) {
    var thumbsId = $('#thumbs' + thumbs + '_' + id);
    $('#thumbs1_' + id)[0].className = $('#thumbs1_' + id)[0].className.replace("active", "")
    $('#thumbs2_' + id)[0].className = $('#thumbs2_' + id)[0].className.replace("active", "")
    $('#thumbs3_' + id)[0].className = $('#thumbs3_' + id)[0].className.replace("active", "")

    thumbsId[0].className = thumbsId[0].className + "active"
}

TeamDet.prototype.SubmitClicked = function (practiceId) {
    var memberId = this.sessionID;
    if (memberId == undefined || memberId == '') { alert('Session has been expired!'); window.location.href = '../Journey/Index'; }
    else if ($('.practice_' + practiceId).length != $('.practice_' + practiceId + ' .active').length) { alert('You have missing observation details'); }
    else {
        var missionId = $('#missionId').val();
        var array = new Array();
        for (i = 0 ; i < $('.practice_' + practiceId + ' .active').length; i++) {
            var thumbs = $('.practice_' + practiceId + ' .active')[i].id.split('_');
            var measureId = thumbs[1];
            var assesment = 0;
            var missionAssesmentId = $('#Assessment_' + measureId).val();

            switch (thumbs[0]) {
                case 'thumbs1':
                    assesment = 5;
                    break;
                case 'thumbs2':
                    assesment = 3;
                    break;
                case 'thumbs3':
                    assesment = 1;
                    break;
            }

            if (assesment > 0) {
                array.push({
                    missionId: missionId,
                    memberId: memberId,
                    measureId: measureId,
                    assesment: assesment,
                    missionAssesmentId: missionAssesmentId
                });
            }
        }

        var model = { Assesments: array };
        $.ajax({
            url: "/api/MissionAssesment",
            type: 'post',
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data.status) {
                    alert("Thanks for the Feedback...");
                    window.location.href = "../mission/observe";
                }
                else alert(result.data.message);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                window.location.href = "../mission/observe";
            }
        });
    }

}

TeamDet.prototype.DeclineClicked = function (missionpracticeId) {
    var memberId = this.sessionID;
    if (memberId == undefined || memberId == '') { alert('Session has been expired!'); window.location.href = '../Journey/Index'; }
    else if ($('.practice_' + practiceId).length != $('.practice_' + practiceId + ' .active').length) { alert('You have missing observation details'); }
    else {
        var model = { MissionPracticeId: missionpracticeId, MemberId: memberId }
        $.ajax({
            url: "/api/DeclineAssesment",
            type: 'post',
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data.status) {
                    window.location.href = "../mission/observe";
                }
                else alert(result.data.message);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                window.location.href = "../mission/observe";
            }
        });
    }

}

function submitObservation(practiceId) {
    var _TeamDet = new TeamDet(sessionId);
    _TeamDet.SubmitClicked(practiceId);
}

function declineObservation(missionpracticeId) {
    var _TeamDet = new TeamDet(sessionId);
    _TeamDet.DeclineClicked(missionpracticeId);
}