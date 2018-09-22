class Clock extends React.Component {
  render() {
    return (
      <div>
        <h2>{this.props.date.toLocaleTimeString()}</h2>
      </div>
    );
  }
}

const customStyles = {
     content : {
      margin: 'auto',
      width: '60%',
      border: '3px solid neon',
      padding: '10px'
     }
 
  };

  const docStyle = {
      margin : 'auto',
      textAlign : 'center'
  }

  class WatchScreen extends React.Component {
    render(){
      return(
        <div>
          <ExampleApp {...props} content="Alexa Responses"/>
          <SignalReact/>                     
        </div>
      );
    } 
  }

  setInterval(tick, 1000);

  class SignalReact extends React.Component {


    componentWillMount () {

      $(function () {

        var mUrl = "https://evryonehuskvarna.sharepoint.com/sites/dev_alexams/Delade%20dokument/Forms/AllItems.aspx?viewpath=%2Fsites%2Fdev%5Falexams%2FDelade%20dokument%2FForms%2FAllItems%2Easpx&id=%2Fsites%2Fdev%5Falexams%2FDelade%20dokument%2Fmarketing%20document%2Edocx&parent=%2Fsites%2Fdev%5Falexams%2FDelade%20dokument";
        var chat = $.connection.chatHub;

        chat.client.addNewMessageToPage = function (name, message) {
          $('#discussion').append('<h2><strong>' + htmlEncode(name)
          + '</strong>: ' + '<br/>' + htmlEncode(message) + '</h2>' + '<br/>');
          
          //if(message === "marketing document.docx") {
          //  window.location.replace(mUrl);
          //}

          window.scrollTo(0, document.body.scrollHeight);
        };

        $('#message').focus();

        $.connection.hub.start().done(function () {
          console.log("Hub connection successful!")
            $('#sendmessage').click(function () {

              chat.server.send($('#displayname').val(), $('#message').val());

              $('#message').val('').focus();

            });
        });
    });

    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }
    
    };

        
    render(){
      return(
        <div className="container">
            <ul id="discussion">
            </ul>
        </div>
      );
    }   
  };

  class ExampleApp extends React.Component {
    constructor () {
      super();
      this.state = {
        showModal: false
      };
      
      this.handleOpenModal = this.handleOpenModal.bind(this);
      this.handleCloseModal = this.handleCloseModal.bind(this);
    }
  

    handleOpenModal () {
      this.setState({ showModal: true});
    }
    
    handleCloseModal () {
      this.setState({ showModal: false });
    }
    
    render (props) {
      return (
        <div>
          <button onClick={this.handleOpenModal}>Trigger Modal</button>
          <ReactModal 
             isOpen={this.state.showModal}
             contentLabel="Minimal Modal Example"
             style={customStyles}
          >
            <button onClick={this.handleCloseModal}>Close Modal</button>

            <div style={docStyle}> 
              {this.props.content}          
              <Clock date={new Date()} />

            </div>
          </ReactModal>
        </div>
      );
    }
  }
  const props = {};
  
  function tick() {
  ReactDOM.render(<WatchScreen/>,          
    document.getElementById('root'))
  };