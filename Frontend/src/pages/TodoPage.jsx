import React, { useState } from 'react';
import { useParams, useLoaderData, useNavigate } from 'react-router-dom';
import axios from 'axios';

export default function TodoPage() {
  const { id } = useParams();
  const { todoData } = useLoaderData();
  const [todo, setTodo] = useState(todoData);
  const [title, setTitle] = useState(todoData.title);
  const [isCompleted, setIsCompleted] = useState(todoData.isCompleted);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleUpdate = async (e) => {
    e.preventDefault();
    try {
      const updatedTodo = { ...todo, title, isCompleted };
      const response = await axios.put(`/api/todo/${id}`, updatedTodo);
      setTodo(response.data);
    } catch (error) {
      setError(error);
    }
  };

  const handleDelete = async () => {
    try {
      await axios.delete(`/api/todo/${id}`);
      navigate('/todo');
    } catch (error) {
      setError(error);
    }
  };

  return (
    <>
      <div>
        <h1>Todo Item {id}</h1>
        {error && <p style={{ color: 'red' }}>Error: {error.message}</p>}
        <form onSubmit={handleUpdate}>
          <div>
            <label>Title:</label>
            <input
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
            />
          </div>
          <div>
            <label>Completed:</label>
            <input
              type="checkbox"
              checked={isCompleted}
              onChange={(e) => setIsCompleted(e.target.checked)}
            />
          </div>
          <button type="submit">Update Todo</button>
        </form>
        <button onClick={handleDelete} style={{ marginTop: '10px', backgroundColor: 'red', color: 'white' }}>
          Delete Todo
        </button>
      </div>
    </>
  );
}