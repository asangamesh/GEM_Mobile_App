
function Mission(teamJourneyIdVal) {
    this.teamJourneyId = teamJourneyIdVal;
}

Mission.prototype.Load = function () {

    $("#btncreateMission").off("click");
    $("#btncreateMission").on("click", $.proxy(this.missionClicked, this));

    $("#btnCreateTeam").off("click");
    $("#btnCreateTeam").on("click", $.proxy(this.CreateTeamClicked, this));

}

Mission.prototype.CreateTeamClicked = function () {
    var name = $("#txtTeamName").val();
    var model = { Name: name, JourneyId: 1, MemberId: 1 };

    if (name == '') { alert('please enter TeamName'); $("#txtTeamName").focus(); }
    else {
        $.ajax({
            url: "/api/team",
            type: 'post',
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.status == "OK") {
                    alert("team created Successfully..!");
                }
                else {
                    alert("team created failed..!");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("You have entered wrong details..!");
            }
        });
    }
}

Mission.prototype.missionClicked = function () {

    var startDate = $("#dateStartdate").val();
    var endDate = $("#dateEnddate").val();
    var endTime = $("#timeEndtime").val();

    var elements = document.getElementById('divPractice').children;
    var practiceIds = new Array();

    for (i = 0; i < elements.length; i++) {
        if (elements[i].classList.length == 3) {
            practiceIds[i] = elements[i].id;
        }
    }

    var endDateTime = endDate + ' ' + endTime;
    var teamJourneyId = this.teamJourneyId;
    var practiceId = practiceIds;

    var model = { startDate: startDate, endDate: endDateTime, teamJourneyId: teamJourneyId, practiceId: practiceId };

    if (startDate == '') { alert('please enter startdate'); $("#dateStartdate").focus(); }
    else if (endDate == '') { alert('please enter enddate'); $("#dateEnddate").focus(); }
    else if (practiceId.length == 0) { alert('please select practices'); }
    else {
        $.ajax({
            url: "/api/Mission",
            type: 'post',
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                    alert(result.data.message);
                    window.location.href = '../Mission/Index?teamJourneyId='+teamJourneyId+'';
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("You have entered wrong details..!");
            }
        });
    }
}
