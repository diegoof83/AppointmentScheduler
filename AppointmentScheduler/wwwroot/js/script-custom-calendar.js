$(document).ready(function () {
    InitializeCalendar();
});

function InitializeCalendar() {
    try {
        $('#calendar').fullCalendar({
            timezone: false,
            header: {
                left: 'prev,next,today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            selectable: true,
            editable: false,
            select: function (event) {
                onShowModal(event, null);
            }
        });

    } catch (e) {
        alert(e);
    }
}

//NEW IMPLEMENTATION IS NOT WORKING
//function InitializeCalendar() {
//    try {
//        var calendarEl = document.getElementById('calendar');
//        var calendar = new FullCalendar.Calendar(calendarEl, {
//            initialView: 'dayGridMonth',
//            headerToolbar: {
//                left: 'prev,next,today',
//                center: 'title',
//                right: 'month,agendaWeek,agendaDay'
//            },
//            selectable: true,
//            editable: false,
//            select: function (event) {
//                onShowModal(event, null);
//            }
//        });
//        calendar.render();
//    } catch (e) {
//        alert(e);
//    }
//}

function onShowModal(obj, isEventDetail) {
    $("#appointmentInput").modal("show");
}

function onCloseModal() {
    $("#appointmentInput").modal("hide");
}

function onSubmitFormModal() {
    $("#appointmentInput").modal("hide");