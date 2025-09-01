const getTodos = document.getElementById('getTodos');
const todosContainer = document.getElementById('todosContainer');

getTodos.addEventListener('click', () => {
  fetch('https://jsonplaceholder.typicode.com/todos')
    .then(response => response.json())
    .then(todos => {
      todos.forEach(todo => {

        todosContainer.innerHTML += `
          <h5>${todo.title}</h5>
          <p>Completed: ${todo.completed}</p>
        `;
      });
    });
});