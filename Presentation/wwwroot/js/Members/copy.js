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
    });

    $("#fromGradeId").change(function () {
        $("#searchFromClass").submit();
    });

    $("#fromClassroomId").change(function () {
        $("#searchFromClass").submit();
    });

    $("#fromUsertypeId").change(function () {
        $("#searchFromClass").submit();
    });

    $("#toOrgId").change(function () {
        $("#searchToClass").submit();
    });

    $("#toSchoolId").change(function () {
        $("#searchToClass").submit();
    });

    $("#toSeasonId").change(function () {
        $("#searchToClass").submit();
    });

    $("#toGradeId").change(function () {
        $("#searchToClass").submit();
    });

    $("#toClassroomId").change(function () {
        $("#searchToClass").submit();
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

});