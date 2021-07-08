var routeURL = location.protocol + "//" + location.host;

$(document).ready(function () {
    $("#startTime").kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });

    InitializeCalendar();
});

var calendar;
function InitializeCalendar() {
    try {
        var calendarEl = document.getElementById('calendar');
        if (calendarEl != null) {
            calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                headerToolbar:
                {
                    left: 'prev,next,today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay'
                },
                selectable: true,
                editable: false,
                select: function (event) {
                    onShowModal(event, null);
                },
                eventDisplay: 'block',
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: routeURL + '/api/Appointment/GetAppointments?providerId=' + $("#serviceProviderId").val(),
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (response) {
                            var events = [];
                            if (response.status === 1) {
                                $.each(response.model, function (i, data) {
                                    events.push({
                                        title: data.title,
                                        description: data.description,
                                        start: data.startTime,
                                        end: data.endTime,
                                        backgroundColor: data.isApproved ? "#28a745" : "#dc3545", //red or green color
                                        borderColor: "#162466",
                                        textColor: "white",
                                        id: data.id
                                    });
                                })
                            }
                            successCallback(events);
                        },
                        error: function (xhr) {
                            $.notify("Error", "error");
                        }
                    });
                },
                eventClick: function (info) {
                    getEventDetails(info.event);
                }
            });
            calendar.render();
        }
    } catch (e) {
        alert(e);
    }
}

function getEventDetails(info) {
    $.ajax({
        url: routeURL + '/api/Appointment/GetAppointment/' + info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1 && response.model !== undefined) {
                onShowModal(response.model, true);
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onShowModal(obj, isEventDetail) {
    if (isEventDetail != null) {//Updating new appointment    
        $("#id").val(obj.id);
        $("#title").val(obj.title);
        $("#description").val(obj.description);
        $("#startTime").val(obj.startTime);
        $("#duration").val(obj.duration);
        $("#serviceProviderId").val(obj.serviceProviderId);
        $("#clientId").val(obj.clientId);
        $("#clientName").html(obj.clientName);
        $("#serviceProviderName").html(obj.serviceProviderName);

        if (obj.isApproved) {
            $("#status").html('Approved');
            $("#btnConfirm").addClass('d-none');
            $("#btnSubmit").addClass('d-none');
        }
        else {
            $("#status").html('Pending');
            $("#btnConfirm").removeClass('d-none');
            $("#btnSubmit").removeClass('d-none');
        }

        $("#btnDelete").removeClass('d-none');
    }
    else {//Adding new appointment
    
        $("#id").val(0);
        $("#startTime").val(obj.startStr + " " + new moment().format("hh:mm A"));//gets the date/time which was selected/clicked
        $("#btnDelete").addClass('d-none');        
        $("#btnSubmit").removeClass('d-none');
    }

    $("#appointmentInput").modal("show");
}

function onCloseModal() {
    $("#appointmentForm")[0].reset();
    $("#id").val(0);
    $("#title").val('');
    $("#description").val('');
    $("#startTime").val('');
    $("#duration").val('');
    $("#appointmentInput").modal("hide");
}

function onSubmitModal() {
    if (checkValidation()) {
        var requestData = {
            Id: parseInt($("#id").val()),
            Title: $("#title").val(),
            Description: $("#description").val(),
            StartTime: $("#startTime").val(),
            Duration: $("#duration").val(),
            ServiceProviderId: $("#serviceProviderId").val(),
            ClientId: $("#clientId").val()
        };

        $.ajax({
            url: routeURL + '/api/Appointment/Book',
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: 'application/json',
            success: function (response) {
                if (response.status === 1 || response.status === 2) {
                    calendar.refetchEvents();
                    $.notify(response.message, "success");
                    onCloseModal();
                }
                else {
                    $.notify(response.message, "error");
                }
            },
            error: function (xhr) {
                $.notify("Error", "error");
            }
        });
    }
}

function checkValidation() {
    var isValid = true;

    if ($("#title").val() === undefined || $("#title").val() === "") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $("#title").removeClass('error');
    }

    if ($("#startTime").val() === undefined || $("#startTime").val() === "") {
        isValid = false;
        $("#startTime").addClass('error');
    }
    else {
        $("#startTime").removeClass('error');
    }

    return isValid;
}

function onDeleteModal() {
    var id = $("#id").val();
    $.ajax({
        url: routeURL + '/api/Appointment/Delete/' + id,
        type: 'DELETE',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1) {
                calendar.refetchEvents();
                $.notify(response.message, "success");
                onCloseModal();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onConfirmModal() {
    var id = $("#id").val();
    $.ajax({
        url: routeURL + '/api/Appointment/Confirm/' + id,
        type: 'PUT',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1) {
                calendar.refetchEvents();
                $.notify(response.message, "success");
                onCloseModal();
            }
            else {
                $.notify(response.message, "error");
            }
        },
        error: function (xhr) {
            $.notify("Error", "error");
        }
    });
}

function onServiceProviderSelection() {
    calendar.refetchEvents();
}