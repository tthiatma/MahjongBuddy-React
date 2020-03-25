import React from 'react'
import {Form as FinalForm, Field} from 'react-final-form'
import { Form, Button } from 'semantic-ui-react'
import TextInput from '../../app/common/form/TextInput'


export const LoginForm = () => {
    return (
      <FinalForm
        onSubmit={values => console.log(values)}
        render={({ handleSubmit }) => (
          <Form onSubmit={handleSubmit}>
            <Field name='email' component={TextInput} placeholder='Email' />
            <Field
              name='password'
              component={TextInput}
              placeholder='Password'
              type='password'
            />
            <Button positive content='Login' />
          </Form>
        )}
      />
    );
}
