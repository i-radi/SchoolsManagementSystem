window.onload = function () {
    $('#loadingSpinner').hide();
};

$(function () {
    $('#upload-excel-button').on('click', function () {
        $('#loadingSpinner').show();
    });
});

$(document).ready(function () {
    function updateHiddenInputs() {
        var orgId = $('#OrgId').val();
        var schoolId = $('#SchoolId').val();
        var gradeId = $('#GradeId').val();
        var classroomId = $('#ClassroomId').val();
        var seasonId = $('#SeasonId').val();
        var userTypeId = $('#UserTypeId').val();

        $('#organizationId').val(orgId);
        $('#schoolId').val(schoolId);
        $('#gradeId').val(gradeId);
        $('#classroomId').val(classroomId);
        $('#seasonId').val(seasonId);
        $('#userTypeId').val(userTypeId);
    }

    $('#OrgId, #SchoolId, #GradeId, #ClassroomId, #SeasonId, #UserTypeId')
        .on('change', function () {
            updateHiddenInputs();
        });

    updateHiddenInputs();

    $('#OrgId').on('change', function () {
        var selectedOrganizationId = $(this).val();
        var schoolDropdown = $('#SchoolId');

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

    $('#SchoolId').on('change', function () {
        var selectedSchoolId = $(this).val();

        var gradeDropdown = $('#GradeId');
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

        var seasonDropdown = $('#SeasonId');
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

    $('#GradeId').on('change', function () {
        var selectedGradeId = $(this).val();
        var classroomDropdown = $('#ClassroomId');

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