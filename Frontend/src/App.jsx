import  {
  createBrowserRouter,
  RouterProvider,
} from 'react-router-dom'
import axios from "axios";
import './App.css'
import TodoList from './pages/TodoList'
import TodoPage from './pages/TodoPage'
import Layout from './Layout'

axios.defaults.baseURL = 'https://orange-space-system-5gx7j9j5xp9qf7957-5194.app.github.dev';

const routes = [{
  path: '/',
  element: <Layout></Layout>,
  children: [{
    path: '/todo',
    element: <TodoList></TodoList>,
    loader: async () => {
      const response = await axios.get(`/api/todo`);
      const todoData = response.data;
      return { todoData };
    }
  }, {
    path: '/todo/:id',
    element: <TodoPage></TodoPage>,
    loader: async ({ params }) => {
      const response = await axios.get(`/api/todo/${params.id}`);
      const todoData = response.data;
      return { todoData };
    }
  }]
}]
  
const router = createBrowserRouter(routes);

function App() {
  return (
    <>
      <RouterProvider router={router}></RouterProvider>
    </>
  );
}

export default App
