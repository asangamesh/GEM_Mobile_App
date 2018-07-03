
function TeamDet() {

}

TeamDet.prototype.Load = function () {
    $("#btnCreateTeam").off("click");
    $("#btnCreateTeam").on("click", $.proxy(this.CreateTeamClicked, this));
}

TeamDet.prototype.CreateTeamClicked = function () {

    var name = $("#txtTeamName").val();
    var model= {Name: name, JourneyId : 1, MemberId : 39 };

    if (name == '') { alert('please enter TeamName'); $("#txtTeamName").focus(); }
    else {
        $.ajax({
            url: "../api/team",
            type: 'post',
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.status == "success") {
                    alert(result.message);
                }
                else {
                    alert(result.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("You have entered incorrect username and password");
            }
        });
    }
}
