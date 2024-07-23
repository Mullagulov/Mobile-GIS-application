require('dotenv').config()

const express = require('express')
const radiatorRouter = require('./routers/radiatorRouter')
const pipeRouter = require('./routers/pipeRouter')

const PORT = process.env.PORT || 5000

const app = express()

app.use(express.json())

app.use('/api', radiatorRouter)
app.use('/api', pipeRouter)

app.listen(PORT, () => console.log(`server started ${PORT}`))
