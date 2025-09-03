const getTodos = document.getElementById('getTodos');
const todosContainer = document.getElementById('todosContainer');
let start = 0;
let end = 10;
let todo = [];

fetch('https://jsonplaceholder.typicode.com/todos')
    .then(response => response.json())
    .then(data => {
      todo = data;
    });

getTodos.addEventListener('click', () => {
  for (let i = start; i < end && i < todo.length; i++) {
    todosContainer.innerHTML += `
      <div class="todo-item ${todo[i].completed ? "bg-success" : "bg-danger"} p-2 mb-2 rounded">
        <h6>Todo ID: ${todo[i].id}</h6>
        <h5>${todo[i].title}</h5>
        <p>Completed: ${todo[i].completed}</p>
      </div>
    `;
  }
  start += 10;
  end += 10;
});