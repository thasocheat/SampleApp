//Image preview function
function showImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}


// Pet post function ajax
function PetPost(form) {
    $.validator.unobtrusive.parse(form);

    // If it's not an update
    if ($(form).valid()) {
        var ajaxCondig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {

                // Check if response success
                if (response.success) {
                    //$("#view").html(response);

                    // Past the refresh to the tab
                    refreshAddNewTab($(form).attr('data-resetUrl'), true);

                    refreshViewAllTab();

                    // Assuming the server returns JSON data
                    // Parse the JSON data to get the pets
                    //var petsData = JSON.parse(data);

                    //// Display the pets in the table
                    //displayPets(petsData);

                    // Display success message using Toastr
                    toastr.success(response.message);
                } else {
                    // Display error message using Toastr
                    toastr.error(response.message);
                }
                //handleAddOrUpdatePetResponse(response);
            }
        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxCondig["contentType"] = false;
            ajaxCondig["processData"] = false;
        }
        $.ajax(ajaxCondig);
    }
    return false;

    // Check if it's an update operation
    if ($('#submitButton').val() == 'Update Pet') {

        // Check if the ImageUploaad field has a file
        if (!formData.has("ImageUpload")) {
            // If no file remove the ImageUpload key from form
            formData.delete("ImageUpload");
        }


        // Perform the update operation
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    //$("#view").html(response);

                    // Past the refresh to the tab
                    refreshAddNewTab($(form).attr('data-resetUrl'), true);

                    refreshViewAllTab();

                    // Update was successful
                    // Showing the message
                    toastr.success(respoq.message);

                    // Reset the form
                    ResetForm();
                } else {
                    // Update failed
                    toastr.error(respoq.message);
                }
                //handleAddOrUpdatePetResponse(response);
            },
            error: function (xhr, status, error) {
                // Handle the case where the AJAX request encounters an error
                toastr.error("An error occurred while updating the pet: " + error);
            }
        });
        // Return false
        return false;



    }

}

// Delete function
function confirmDelete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'You won\'t be able to revert this!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // If the user clicks "Yes," proceed with the deletion
            deleteItem(url);
        }
    });
}

function deleteItem(url, itemId) {
    $.ajax({
        type: 'POST',
        url: url,
        success: function (response) {
            if (response.success) {
                // Show success message
                Swal.fire({
                    title: 'Deleted!',
                    text: 'Your item has been deleted.',
                    timer: 2e3,
                    icon: 'success'
                }).then(() => {
                    // Optionally, refresh the page or update the view after deletion
                    location.reload();

                    //// Remove the deleted item from the form
                    //var deletedPetID = response.deletedPetID;
                    //$('#item-' + deletedPetID).remove();
                    $('#item-' + itemId).remove();

                    refreshViewAllTab();

                    //$("#example").html(response.html);
                });
            } else {
                // Show error message
                Swal.fire({
                    title: 'Error!',
                    text: response.message,
                    icon: 'error'
                });
            }
        },
        error: function () {
            // Show error message if the AJAX request encounters an error
            Swal.fire({
                title: 'Error!',
                text: 'An error occurred while deleting the item.',
                icon: 'error'
            });
        }
    });
}


// Function to switch to the "View All" tab
function switchToViewTab() {
    $('ul.nav.nav-tabs a[href="#view"]').tab('show');
    // Load the content of the "View All" tab
    $.ajax({
        url: '@Url.Action("ViewAll", "Pet")', 
        //url: '@Url.Action("Index", "Home")', 
        //url: viewAllUrl,
        type: 'GET',
        success: function (data) {
            //$("#view").html(data);
            //$("#example").html(data.html);

            // Assuming the server returns JSON data
            // Parse the JSON data to get the pets
            //var petsData = JSON.parse(data);

            //// Display the pets in the table
            //displayPets(petsData);


            //loadData();

        },
        //error: function () {
        //    toastr.error('An error occurred while loading the "View All" content.', 'Error');
        //}
    });
}


function displayPets(pets) {
    $("#petTableBody").empty();

    //var petTableList = $("#petTableBody");
    //var petName = $("#petNameInput").val();
    //var petAge = $("#petAgeInput").val();
    //var petGender = $("#petGenderInput option:selected").text();
    //var petImages = $("#imagePreview").attr('src');

    $.each(pets, function (index, pet) {
        var row = "<tr>";
        //row += "<td>" + pet.PetID + "</td>";
        row += "<td>" + pet.PetName + "</td>";
        row += "<td>" + pet.PetAge + "</td>";
        row += "<td>" + pet.PetGender + "</td>";
        row += "<td><img src='" + pet.PetImagePath + "' alt='Pet Image' width='50' height='50'></td>";
        row += "<td><a class='btn btn-primary btn-sm' href='" + '@Url.Action("AddOrEdit", "Pet", new { id = ' + pet.PetID + ' })' + "'><i class='fa fa-pencil fa-lg'></i></a><a class='btn btn-danger btn-sm' onclick='confirmDelete(\"" + '@Url.Action("Delete", "Pet", new { id = ' + pet.PetID + ' })' + "\", " + pet.PetID + ")'><i class='fa fa-trash fa-lg'></i></a></td>";
        //row += "<td></td>";
        row += "</tr>";
        $("#petTableBody").append(row);
        //petTableList.append(row);
    });
}

// Call this function after adding, editing, or deleting a pet
function refreshViewAllTab() {
    switchToViewTab();

    //location.reload();
}

// Function to handle the AJAX response and update the table
function handleAddOrUpdatePetResponse(response) {
    if (response.success) {
        // Close the modal
        //$('.bd-example-modal-lg').modal('hide');

        // Update the HTML content of the table with the new data
        //$("#example").html(response.html);

        // Display success message using Toastr
        toastr.success(response.message); 

        // Switch to the "View All" tab
        switchToViewTab();
    } else {
        // Display success message using Toastr
        toastr.error(response.message); 
    }
}





// Refresh form when click add new
function refreshAddNewTab(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            $("#addOredit").html(response);
            $('ul.nav.nav-tabs a:eq(1)').html('Add New');
            if (showViewTab) {
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
            }
                

            //refreshViewAllTab();
        }
    });
}

// Function Edit
function Edit(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {            

            $("#addOredit").html(response);

            //// Change the button Add New into Edit
            //$('ul.nav.nav-tabs a[href="#addOredit"]').tab('show');


            // Change the button Add New into Edit
            $('ul.nav.nav-tabs a:eq(1)').html('Edit');

            // Showing tab
            $('ul.nav.nav-tabs a:eq(1)').tab('show');

            // change button submit to ('Update Pet')
            $('#submitButton').val('Update Pet');

            // Reset form or cancel button 
            $('#cancelButton').on('click', function () {
                ResetForm();
            });
        }
    });
}

// Function to handle cancel operation
function CancelOperation() {
    
    ResetForm();

    // Hide the cancel button
    $('#cancelButton').hide();
}

// Function to reset the form
function ResetForm() {
    // Assuming 'myForm' is the ID of the form
    $('#myForm')[0].reset();

    // Clear the file input
    $('#myForm input[type="file"]').val('');

    // Clear the select input
    $('#myForm select').val('-- Select Gender --');

    // Clear the text input
    $('#myForm input[type="text"]').val('');

    // Clear the image preview
    $('#imagePreview').attr('src', '/AppFiles/Images/default.png');

    // Change the button back to submit
    $('#submitButton').val('Submit');

    // Change the button behavior back to submit
    $('#myForm').attr('onsubmit', 'return PetPost(this);');

    // Change the button text back to 'Add New'
    $('ul.nav.nav-tabs a:eq(1)').html('Add New');
}

// Function to handle edit operation
function EditOperation(petID) {
    // Set the PetID in the hidden field
    $('#petID').val(petID);

    // Set the submit button text to 'Updaate Pet'
    $('#submitButton').val('Update Pet');

    // Showing the cancel button
    $('#cancelButton').show();
}


