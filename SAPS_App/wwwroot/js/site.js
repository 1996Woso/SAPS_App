function Alphabets(input) {
    let pattern = /^[a-zA-Z]+$/;
    return pattern.test(input);
}
function validID(suspectId) {
    // Check if the suspectId is a non-empty string
    if (typeof suspectId !== 'string' || suspectId.length === 0) {
        return "SuspectId must be a non-empty string.";
    }

    // Check if the suspectId has 13 characters
    if (suspectId.length !== 13) {
        return "The SuspectId must have 13 characters.";
    }

    // Extract the first 6 characters of the SuspectId
    const datePrefix = suspectId.substring(0, 6);

    // Check if the first six characters are numeric
    if (!/^\d{6}$/.test(datePrefix)) {
        return "The first six characters must be numeric.";
    }

    const today = new Date();
    const this_year = today.getFullYear();

    // Attempt to parse the date
    let testDate;
    if (this_year - 2000 - parseInt(datePrefix.substring(0, 2), 10) < 0) {
        testDate = new Date(1900 + parseInt(datePrefix.substring(0, 2), 10), parseInt(datePrefix.substring(2, 4), 10) - 1, parseInt(datePrefix.substring(4, 6), 10));
    } else {
        testDate = new Date(2000 + parseInt(datePrefix.substring(0, 2), 10), parseInt(datePrefix.substring(2, 4), 10) - 1, parseInt(datePrefix.substring(4, 6), 10));
    }

    // Check if it's a valid date
    if (
        testDate.getFullYear() === (2000 + parseInt(datePrefix.substring(0, 2), 10)) || testDate.getFullYear() === (1900 + parseInt(datePrefix.substring(0, 2), 10)) &&
        testDate.getMonth() === parseInt(datePrefix.substring(2, 4), 10) - 1 &&
        testDate.getDate() === parseInt(datePrefix.substring(4, 6), 10)
    ) {
        return true; // Valid date prefix
    } else {
        return "The first six characters must represent a valid date (YYMMDD).";
    }

   
}
function allowDigits(event) {
    let inputChar = String.fromCharCode(event.which);
    let digits = /[0-9]/;
    if (!digits.test(inputChar)) {
        event.preventDefault();
    }
}
function allowAlphabets(event) {
    let inputChar = String.fromCharCode(event.which);
    let alphabets = /[A-Za-z]+$/;
    if (!alphabets.test(inputChar)) {
        event.preventDefault();
    }
}
function letterCapitalize(inputId) {
    let input = document.getElementById(inputId);
    let value = input.value.toLowerCase().split(' ');
    let capitalised_input = value.map(c => c.charAt(0).toUpperCase() + c.slice(1));
    capitalised_input = capitalised_input.join(" ");
    input.value = capitalised_input;
}
function LowerCase(inputId) {
    let input = document.getElementById(inputId);
    let value = input.value.toLowerCase().split(' ');
    input.value = value.join(" ");
}
function pasteEmail(event, inputId, maxLength) {
    event.preventDefault();
    let pastedText = (event.originalEvent || event).clipboardData.getData('text/plain').toLowerCase();
    pastedText = pastedText.replace(/@joburg\.org\.za/g, '');
    pastedText = pastedText.replace(/[^a-zA-Z0-9]/g, '');
    let l = "@joburg.org.za".length;
    if (pastedText.length > maxLength - l) {
        pastedText = pastedText.slice(0, pastedText.length - (pastedText.length - maxLength) - l) + "@joburg.org.za";
    }
    else {
        pastedText = pastedText.slice(0, maxLength) + "@joburg.org.za";
    }
    document.getElementById(inputId).value = pastedText;
    return false;
}
function pasteAlphabets(event, inputId, maxLength) {
    event.preventDefault();
    let pastedText = (event.originalEvent || event).clipboardData.getData('text/plain').toLowerCase();
    let only_alphabets = pastedText.replace(/[^A-Za-z]/g, '');
    only_alphabets = only_alphabets.split(' ').map(c => c.charAt(0).toUpperCase() + c.slice(1)).join(" ");
    if (only_alphabets.length > maxLength) {
        only_alphabets = only_alphabets.slice(0, maxLength);
    }
    document.getElementById(inputId).value = only_alphabets;
    return false;
}
function pasteDigits(event, inputId, maxLength) {
    event.preventDefault();
    let pastedText = (event.originalEvent || event).clipboardData.getData('text/plain');
    let only_digits = pastedText.replace(/[^0-9]/g, '');
    if (only_digits.length > maxLength) {
        only_digits = only_digits.slice(0, maxLength);
    }
    document.getElementById(inputId).value = only_digits;
    return false;
}
function letterCapitalize(inputId) {
    let input = document.getElementById(inputId);
    let value = input.value.toLowerCase().split(' ');
    let capitalised_input = value.map(c => c.charAt(0).toUpperCase() + c.slice(1));
    capitalised_input = capitalised_input.join(" ");
    input.value = capitalised_input;
}
function pasteAlphabets(event, inputId, maxLength) {
    event.preventDefault();
    let pastedText = (event.originalEvent || event).clipboardData.getData('text/plain').toLowerCase();
    let only_alphabets = pastedText.replace(/[^A-Za-z]/g, '');
    only_alphabets = only_alphabets.split(' ').map(c => c.charAt(0).toUpperCase() + c.slice(1)).join(" ");
    if (only_alphabets.length > maxLength) {
        only_alphabets = only_alphabets.slice(0, maxLength);
    }
    document.getElementById(inputId).value = only_alphabets;
    return false;
}
function pasteDigits(event, inputId, maxLength) {
    event.preventDefault();
    let pastedText = (event.originalEvent || event).clipboardData.getData('text/plain');
    let only_digits = pastedText.replace(/[^0-9]/g, '');
    if (only_digits.length > maxLength) {
        only_digits = only_digits.slice(0, maxLength);
    }
    document.getElementById(inputId).value = only_digits;
    return false;
}

document.addEventListener('DOMContentLoaded', function () {
    //const editForm = document.querySelector('.edit-form');
    //if (editForm) {
    //    editForm.addEventListener('submit', async function (event) {
    //        event.preventDefault();
    //        EditForm(editForm)
    //    });
    //}

    var editForms = document.querySelectorAll('.edit-form');
    editForms.forEach(function (editForm) {
        editForm.removeEventListener('submit', EditForm);
        editForm.addEventListener('submit', EditForm);
    });

 
});
async function EditForm(event) {
    event.preventDefault();
    var inputs = this.querySelectorAll('.edit-form-input');
    var allFilled = true;

    inputs.forEach(function (input) {
        if (!input.value) {
            allFilled = false;
        }
    });
    if (allFilled) {
        const formData = new FormData(this);
        const actionUrl = this.getAttribute('data-action-url');
        const response = await fetch(actionUrl, {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const result = await response.json();
            Swal.fire({
                icon: 'success',
                text: result.message,
                showConfirmButton: false,
                allowOutsideClick: false,
                timer: 4000,
            }).then(() => {
                window.location.href = result.redirectUrl;
            });
        }
        else {
            const result = await response.json();
            Swal.fire({
                icon: 'error',
                text: `Failed. ${result.message}`,
                allowOutsideClick: false,
                showConfirmButton: true,
            });
        }

    }
    else {
        Swal.fire({
            icon: 'error',
            title: 'Unfilled field(s)',
            text: 'Please fill all the required fields!',
            allowOutsideClick: false,
            showConfirmButton: true,
        });
    }
}
//async function EditForm(form) {
//    var inputs = form.querySelectorAll('.edit-form-input');
//    var allFilled = true;

//    inputs.forEach(function (input) {
//        if (!input.value) {
//            allFilled = false;
//        }
//    });
//    if (allFilled) {
//        const formData = new FormData(form);
//        const actionUrl = form.getAttribute('data-action-url');
//        const response = await fetch(actionUrl, {
//            method: 'POST',
//            body: formData
//        });

//        if (response.ok) {
//            const result = await response.json();
//            Swal.fire({
//                icon: 'success',
//                text: result.message,
//                showConfirmButton:false,
//                allowOutsideClick: false,
//                timer: 4000,
//            }).then(() => {
//                window.location.href = result.redirectUrl;
//            });
//        }
//        else {
//            const result = await response.json();
//            Swal.fire({
//                icon: 'error',
//                text: `Failed. ${result.message}`,
//                allowOutsideClick: false,
//                showConfirmButton: true,
//            });
//        }

//    }
//    else {
//        Swal.fire({
//            icon: 'error',
//            title: 'Unfilled field(s)',
//            text: 'Please fill all the required fields!',
//            allowOutsideClick: false,
//            showConfirmButton: true,
//        });
//    }
//}