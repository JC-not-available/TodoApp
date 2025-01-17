import React, { useState } from 'react';
import { useParams, useLoaderData, useNavigate } from 'react-router-dom';
import axios from 'axios';

export default function TodoPage() {
  const { id } = useParams();
  const { todoData } = useLoaderData();
  const [title, setTitle] = useState(todoData.title);
  const [isCompleted, setIsCompleted] = useState(todoData.isCompleted);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleUpdate = async (e) => {
    e.preventDefault();
    try {
      const updatedTodo = { id: todoData.id, title, isCompleted };
      const response = await axios.put(`/api/todo/${id}`, updatedTodo);
      // Update the state with the latest todo data
      todoData.title = response.data.title;
      todoData.isCompleted = response.data.isCompleted;
      setError(null);
    } catch (error) {
      if (error.response) {
        setError(error.response.data);
      } else if (error.request) {
        setError("No response received from the server.");
      } else {
        setError("An error occurred while setting up the request: " + error.message);
      }
    }
  };

  const handleDelete = async () => {
    try {
      await axios.delete(`/api/todo/${id}`);
      navigate('/todo');
    } catch (error) {
      if (error.response) {
        setError(error.response.data);
      } else if (error.request) {
        setError("No response received from the server.");
      } else {
        setError("An error occurred while setting up the request: " + error.message);
      }
    }
  };

  return (
    <>
      <div>
        <h1>Todo Item {id}</h1>
        {error && <p style={{ color: 'red' }}>Error: {error}</p>}
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
