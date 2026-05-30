import React, { useState, useEffect } from 'react';
import { 
  Container, Typography, Paper, List, ListItem, ListItemText, 
  CircularProgress, Button, TextField, Dialog, DialogTitle, 
  DialogContent, DialogActions, Box, IconButton
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import api from '../services/api';

const TaskList = () => {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [openDialog, setOpenDialog] = useState(false);
  const [newTask, setNewTask] = useState({ title: '', description: '' });

  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = async () => {
    try {
      setLoading(true);
      const response = await api.get('/taskapi');
      setTasks(response.data);
      setError('');
    } catch (err) {
      console.error('Ошибка при загрузке задач:', err);
      setError('Не удалось загрузить задачи');
    } finally {
      setLoading(false);
    }
  };

  const handleCreateTask = async () => {
    if (!newTask.title.trim()) {
      alert('Введите название задачи');
      return;
    }

    try {
      const response = await api.post('/taskapi', {
        title: newTask.title,
        description: newTask.description,
        status: 'New',
        priority: 'Medium'
      });
      
      setTasks([response.data, ...tasks]);
      setOpenDialog(false);
      setNewTask({ title: '', description: '' });
    } catch (err) {
      console.error('Ошибка при создании задачи:', err);
      alert('Не удалось создать задачу');
    }
  };

  const handleDeleteTask = async (id) => {
    if (window.confirm('Удалить эту задачу?')) {
      try {
        await api.delete(`/taskapi/${id}`);
        setTasks(tasks.filter(task => task.id !== id));
      } catch (err) {
        console.error('Ошибка при удалении:', err);
        alert('Не удалось удалить задачу');
      }
    }
  };

  if (loading) {
    return (
      <Container style={{ textAlign: 'center', marginTop: '50px' }}>
        <CircularProgress />
      </Container>
    );
  }

  return (
    <Container maxWidth="md" style={{ marginTop: '30px' }}>
      <Paper style={{ padding: '20px' }}>
        <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
          <Typography variant="h4">
            Мои задачи
          </Typography>
          <Button 
            variant="contained" 
            color="primary"
            onClick={() => setOpenDialog(true)}
          >
            + Новая задача
          </Button>
        </Box>
        
        {tasks.length === 0 ? (
          <Typography>Нет задач. Создайте первую задачу!</Typography>
        ) : (
          <List>
            {tasks.map((task) => (
              <ListItem key={task.id} divider>
                <ListItemText
                  primary={task.title}
                  secondary={task.description || 'Без описания'}
                />
                <IconButton 
                  edge="end" 
                  color="error"
                  onClick={() => handleDeleteTask(task.id)}
                >
                  <DeleteIcon />
                </IconButton>
              </ListItem>
            ))}
          </List>
        )}
      </Paper>

      {/* Диалог создания задачи */}
      <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
        <DialogTitle>Новая задача</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            margin="dense"
            label="Название задачи"
            fullWidth
            value={newTask.title}
            onChange={(e) => setNewTask({ ...newTask, title: e.target.value })}
          />
          <TextField
            margin="dense"
            label="Описание"
            fullWidth
            multiline
            rows={3}
            value={newTask.description}
            onChange={(e) => setNewTask({ ...newTask, description: e.target.value })}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenDialog(false)}>Отмена</Button>
          <Button onClick={handleCreateTask} variant="contained" color="primary">
            Создать
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default TaskList;