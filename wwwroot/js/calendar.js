<script>
    function createCalendar() {
    // tworzymy element kalendarza i dodajemy go do kontenera
    var calendarDiv = $('<div>').attr('id', 'calendar');
        $('#calendarContainer').append(calendarDiv);

        // konfigurujemy kalendarz z wyborem daty i godziny
        $('#calendar').datetimepicker({
            dateFormat: 'yy-mm-dd',
        timeFormat: 'HH:mm:ss',
        onClose: function(dateText, inst) {
            // ustawiamy warto�� pola hidden z wybran� dat� po zamkni�ciu kalendarza
            $('#selectedDate').val(dateText);
      }
    });
  }

        $(document).ready(function() {
            createCalendar();
  });
</script>