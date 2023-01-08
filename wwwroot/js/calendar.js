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
            // ustawiamy wartoœæ pola hidden z wybran¹ dat¹ po zamkniêciu kalendarza
            $('#selectedDate').val(dateText);
      }
    });
  }

        $(document).ready(function() {
            createCalendar();
  });
</script>