document.getElementById('update-user-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    // Преобразуем чекбокс IsAdmin в true/false
    data.IsAdmin = data.IsAdmin === 'on';

    // Преобразуем Id в число
    data.Id = parseInt(data.Id);

    const response = await fetch(form.action, {
        method: form.method,
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    try {
        const result = await response.json();
        if (result.Message != null){
            alert("Error " + result.Message)
            return;
        }
    }
    catch (error){
        console.log(error);
    }
    
    if (response.ok) {
        // После успешного запроса перенаправляем на страницу со списком пользователей
        window.location.href = "/admin"; // Перенаправление на список пользователей
    } else {
        console.error('Ошибка при обновлении пользователя:', response.statusText);
        // Обработка ошибки или уведомление пользователя
    }
});
