import React, { useState } from 'react';
import { useLoaderData, Link, useNavigate } from 'react-router-dom';
import axios from 'axios';

export default function TodoList() {
  const { todoData } = useLoaderData();
  const [todos, setTodos] = useState(todoData);
  const [error, setError] = useState(null);
  const [newTodo, setNewTodo] = useState({ title: '', isCompleted: false });
  const navigate = useNavigate();

  const handleDeleteAll = async () => {
    try {
      await axios.delete('/api/todo/reset');
      setTodos([]);
      navigate('/todo');
    } catch (error) {
      setError(error);
    }
  };

  const handleAddTodo = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/todo', newTodo);
      setTodos([...todos, response.data]);
      setNewTodo({ title: '', isCompleted: false });
    } catch (error) {
      setError(error);
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>Todo List</h1>
      {error && <p style={{ color: 'red' }}>Error: {error.message}</p>}
      <form onSubmit={handleAddTodo} style={{ marginBottom: '20px' }}>
        <div>
          <label>Title:</label>
          <input
            type="text"
            value={newTodo.title}
            onChange={(e) => setNewTodo({ ...newTodo, title: e.target.value })}
            required
            style={{ marginRight: '10px' }}
          />
        </div>
        <div>
          <label>Completed:</label>
          <input
            type="checkbox"
            checked={newTodo.isCompleted}
            onChange={(e) => setNewTodo({ ...newTodo, isCompleted: e.target.checked })}
            style={{ marginRight: '10px' }}
          />
        </div>
        <button type="submit" style={{ padding: '10px', border: 'none', borderRadius: '5px', cursor: 'pointer' }}>
          Add Todo
        </button>
      </form>
      <button onClick={handleDeleteAll} style={{ marginBottom: '20px', backgroundColor: 'red', color: 'white', padding: '10px', border: 'none', borderRadius: '5px', cursor: 'pointer' }}>
        Delete All and Reset
      </button>
      <ul style={{ listStyleType: 'none', padding: 0 }}>
        {todos.map(todo => (
          <li key={todo.id} style={{ marginBottom: '10px', padding: '10px', border: '1px solid #ccc', borderRadius: '5px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Link to={`${todo.id}`} style={{ textDecoration: 'none', color: 'black' }}>{todo.title}</Link> 
            <span>{todo.isCompleted ? '✔️' : '❌'}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}