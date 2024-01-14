$(document).ready(function () {

    $("#schoolId").change(function () {
        $("#searchForm").submit();
    });
    $("#classroomId").change(function () {
        $("#searchForm").submit();
    });
});