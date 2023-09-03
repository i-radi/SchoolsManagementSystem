$(document).ready(function () {
    const userDropdown = $('#userDropdown');
    const userSearchInput = $('#userSearch');
    const originalUserOptions = userDropdown.find('option').toArray();

    userSearchInput.on('input', function () {
        const searchQuery = $(this).val().toLowerCase();

        const filteredOptions = originalUserOptions.filter(function (option) {
            const userName = $(option).text().toLowerCase();
            return userName.includes(searchQuery);
        });

        userDropdown.html(filteredOptions);

        if (searchQuery === '') {
            userDropdown.prepend('<option value="">-- Select Member --</option>');
        }
    });
});

$(document).ready(function () {
    $('#organizationDropdown').on('change', function () {
        const selectedOrganizationId = $(this).val();
        const url = `/Members/Assign?orgid=${selectedOrganizationId}`;

        window.location.href = url;
    });

    $('#schoolDropdown').on('change', function () {
        const selectedOrganizationId = $('#organizationDropdown').val();
        const selectedSchoolId = $(this).val();
        const url = `/Members/Assign?orgid=${selectedOrganizationId}&schoolid=${selectedSchoolId}`;

        window.location.href = url;
    });

    $('#gradeDropdown').on('change', function () {
        const selectedOrganizationId = $('#organizationDropdown').val();
        const selectedSchoolId = $('#schoolDropdown').val();
        const selectedUserId = $('#userDropdown').val();
        const selectedGradeId = $(this).val();
        const url = `/Members/Assign?orgid=${selectedOrganizationId}&schoolid=${selectedSchoolId}&userId=${selectedUserId}&gradeid=${selectedGradeId}`;

        window.location.href = url;
    });
});

$(document).ready(function () {
    const form = $('#assignForm');
    const assignAndAddNewButton = $('#assignAndAddNew');

    assignAndAddNewButton.on('click', function () {
        const formData = form.serialize();

        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: formData,
            success: function () {
                const selectedOrganizationId = $('#organizationDropdown').val();
                const selectedSchoolId = $('#schoolDropdown').val();
                const selectedGradeId = $('#gradeDropdown').val();
                const url = `/Members/Assign?orgid=${selectedOrganizationId}&schoolid=${selectedSchoolId}&gradeid=${selectedGradeId}`;

                window.location.href = url;
            },
            error: function () {
                alert('An error occurred while submitting the form.');
            }
        });
    });
});