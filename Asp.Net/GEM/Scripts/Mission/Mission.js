
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
    
    var startdate = $("#dateStartdate").val();
    var enddate = $("#dateEnddate").val();
    var endtime = $("#timeEndtime").val();

    var elements = document.getElementById('divPractice').children;
    var Practiceids = new Array();
   
    for (i = 0; i < elements.length; i++) {
        if (elements[i].classList.length == 3)
        {
            Practiceids[i] = elements[i].id;
        }
    }

    var enddatetime = enddate + ' ' + endtime;
    var teamjourneyid = this.teamJourneyId;
    var practiceid = Practiceids;

    var model = { startdate: startdate, enddate: enddatetime, teamjourneyid: teamjourneyid, practiceid: practiceid };

    if (startdate == '') { alert('please enter startdate'); $("#dateStartdate").focus(); }
    else if (enddate == '') { alert('please enter enddate'); $("#dateEnddate").focus(); }
    else if (practiceid.length == 0) { alert('Practice selection is missing'); $("#dateEnddate").focus(); }
    else {
        $.ajax({
            url: "/api/Mission",
            type: 'post',
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.data.status == "true") {
                    alert(result.data.message);
                }
                else {
                    alert(result.data.message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("You have entered wrong details..!");
            }
        });
    }
}
