$(document).ready(function () {
    $("#searchUserName").change(function () {
        $("#searchFromClass").submit();
    });

    $("#fromOrgId").change(function () {
        $("#searchFromClass").submit();
    });

    $("#fromSchoolId").change(function () {
        $("#searchFromClass").submit();
    });

    $("#fromSeasonId").change(function () {
        $("#searchFromClass").submit();
        $("#FromSeasonId").val($("#fromSeasonId").val());
    });

    $("#fromGradeId").change(function () {
        $("#searchFromClass").submit();
    });

    $("#fromClassroomId").change(function () {
        $("#searchFromClass").submit();
        $("#FromClassroomId").val($("#fromClassroomId").val());
    });

    $("#fromUsertypeId").change(function () {
        $("#searchFromClass").submit();
    });

    $("#toSeasonId").change(function () {
        $("#ToSeasonId").val($("#toSeasonId").val());
    });

    $("#toClassroomId").change(function () {
        $("#ToClassroomId").val($("#toClassroomId").val());
    });

    $("#copyButton").click(function () {
        var selectedUserIds = [];
        $("input[name='selectedUserIds']:checked").each(function () {
            selectedUserIds.push(parseInt($(this).val()));
        });
        var selectedUserIdsString = selectedUserIds.join(',');
        $("#SelectedUserIds").val(selectedUserIdsString);
        $("#copy").submit();
    });

    $('#selectAllCheckbox').change(function () {
        var isChecked = $(this).is(':checked');
        $('input[name="selectedUserIds"]').prop('checked', isChecked);
    });

    $("#copyButton").prop("disabled", true);

    function checkSelections() {
        var seasonId = $("#toSeasonId").val();
        var classroomId = $("#toClassroomId").val();

        if (seasonId && classroomId) {
            $("#copyButton").prop("disabled", false);
        } else {
            $("#copyButton").prop("disabled", true);
        }
    }

    $("#toSeasonId, #toClassroomId").change(function () {
        checkSelections();
    });
});