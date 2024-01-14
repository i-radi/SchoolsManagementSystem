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

    $('#toOrgId').on('change', function () {
        var selectedOrganizationId = $(this).val();
        var schoolDropdown = $('#toSchoolId');

        schoolDropdown.empty();

        schoolDropdown.append($('<option>').val('').text('Select School'));
        $.ajax({
            url: '/Users/GetSchoolsByOrganization',
            type: 'GET',
            data: { organizationId: selectedOrganizationId },
            success: function (data) {
                $.each(data, function (i, item) {
                    schoolDropdown.append($('<option>').val(item.value).text(item.text));
                });
            }
        });
    });

    $('#toSchoolId').on('change', function () {
        var selectedSchoolId = $(this).val();

        var gradeDropdown = $('#toGradeId');
        gradeDropdown.empty();
        gradeDropdown.append($('<option>').val('').text('Select Grade'));
        $.ajax({
            url: '/Users/GetGradesBySchool',
            type: 'GET',
            data: { schoolId: selectedSchoolId },
            success: function (data) {
                $.each(data, function (i, item) {
                    gradeDropdown.append($('<option>').val(item.value).text(item.text));
                });
            }
        });

        var seasonDropdown = $('#toSeasonId');
        seasonDropdown.empty();
        seasonDropdown.append($('<option>').val('').text('Select Season'));
        $.ajax({
            url: '/Users/GetSeasonsBySchool',
            type: 'GET',
            data: { schoolId: selectedSchoolId },
            success: function (data) {
                $.each(data, function (i, item) {
                    seasonDropdown.append($('<option>').val(item.value).text(item.text));
                });
            }
        });
    });

    $('#toGradeId').on('change', function () {
        var selectedGradeId = $(this).val();
        var classroomDropdown = $('#toClassroomId');

        classroomDropdown.empty();

        classroomDropdown.append($('<option>').val('').text('Select Classroom'));
        $.ajax({
            url: '/Users/GetClassroomsByGrade',
            type: 'GET',
            data: { gradeId: selectedGradeId },
            success: function (data) {
                $.each(data, function (i, item) {
                    classroomDropdown.append($('<option>').val(item.value).text(item.text));
                });
            }
        });
    });
});