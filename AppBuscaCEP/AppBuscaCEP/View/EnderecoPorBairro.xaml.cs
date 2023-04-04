using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppBuscaCEP.Model;
using AppBuscaCEP.Service;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBuscaCEP.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnderecoPorBairro : ContentPage
    {
        Cidade cidade_escolhida;
        ObservableCollection<Cidade> lst_cidades = new ObservableCollection<Cidade>();
        ObservableCollection<Bairro> lst_bairros = new ObservableCollection<Bairro>();
        public EnderecoPorBairro()
        {
            InitializeComponent();
            pck_cidade.ItemsSource = lst_cidades;
            pck_bairro.ItemsSource = lst_bairros;
        }

        private async void pck_estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker disparador = sender as Picker;

                string estado_selecionado = disparador.SelectedItem as string;

                List<Cidade> arr_cidades = await DataService.GetCidadesByUF(estado_selecionado);

                lst_cidades.Clear();

                arr_cidades.ForEach(i => lst_cidades.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void pck_cidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker disparador = sender as Picker;

                Cidade cidade_selecionada = disparador.SelectedItem as Cidade;

                List<Bairro> arr_bairros = await DataService.GetBairrosByIdCidade(cidade_selecionada.id_cidade);

                lst_bairros.Clear();

                arr_bairros.ForEach(i => lst_bairros.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void pck_bairro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                carregando.IsRunning = true;

                lst_endereco.ItemsSource = null;

                Picker disparador = sender as Picker;

                Bairro bairro_selecionado = disparador.SelectedItem as Bairro;

                if (bairro_selecionado != null)
                {
                    List<Logradouro> arr_end = await DataService.GetLogradourosByBairroAndIdCidade(bairro_selecionado.descricao_bairro, cidade_escolhida.id_cidade);

                    lst_endereco.ItemsSource = arr_end;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                carregando.IsRunning = false;
            }
        }
    }
}