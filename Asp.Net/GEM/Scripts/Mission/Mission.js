
function Mission(teamJourneyIdVal) {
    this.teamJourneyId = teamJourneyIdVal;
}

Mission.prototype.Load = function () {

    $("#btncreateMission").off("click");
    $("#btncreateMission").on("click", $.proxy(this.MissionClicked, this));

    jQuery(document).ready(function () {

        jQuery('.navbar-toggle').click(function () {
            jQuery('.navbar-right').toggle();
        });
    });

    $(document).ready(function () {
        $('#accept-practice-btn').click(function () {
            $('#myMemberModal').modal('hide');
        });
        $('#reject-practice-btn').click(function () {
            $('#myMemberModal').modal('hide');
            $('#rejectionModal').modal('show');
        });

        var date_input = $('input[name="date"]'); //our date input has the name "date"
        var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
        date_input.datepicker({
            format: 'mm/dd/yyyy',
            container: container,
            todayHighlight: true,
            autoclose: true,
        })

        $('input.timepicker').timepicker({
            timeFormat: 'hh:mm p',
            // year, month, day and seconds are not important
            minTime: new Date(0, 0, 0, 0, 1, 0),
            maxTime: new Date(0, 0, 0, 23, 59, 0),
            startTime: new Date(0, 0, 0, 0, 0, 0),
            interval: 30,
        });

    });
}

Mission.prototype.MissionClicked = function () {

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
                //alert(result.data.message);
                $.ajax({
                    url: "../Mission/Index?teamJourneyId=" + teamJourneyId,
                    type: 'get',
                    cache: false,
                    async: false,
                    success: function (result) {
                        window.location.href = '../Journey/View';
                    }
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("You have entered wrong details..!");
            }
        });
    }
}

function myFunction() {
    var x = document.getElementById("timeEndtime").defaultValue;
    document.getElementById("demo").innerHTML = x;
}

function SelectdPractice(id) {
    var str = $("#" + id).attr("class");
    if (str.indexOf("active") != -1) {
        $("#" + id).removeClass("active");
        $('#chk_' + id).prop('checked', '')
    }
    else {
        $("#" + id).addClass("active");
        $('#chk_' + id).prop('checked', 'true')
    }
}

function MemberProcess(url, tjmemberId, tjId) {
    $.ajax({
        url: url,
        type: 'get',
        cache: false,
        data: { tjmemberid: tjmemberId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.success = false) window.location.href = '../Mission/Index?teamJourneyId=' + tjId;
            else window.location.href = '../Journey/Index';
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            window.location.href = '../Journey/Index';
        }
    });
}