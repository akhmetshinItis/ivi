document.getElementById('update-actor-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    // Преобразуем данные в правильный формат
    data.Id = parseInt(data.Id);  // Преобразуем Id в число
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
    
    // После успешного запроса перенаправляем на страницу с актерами
    if (response.ok) {
        window.location.href = "/admin";
    } else {
        console.error('Ошибка при обновлении актера:', response.statusText);
        // Можете добавить обработку ошибки или уведомление пользователя
    }
});
