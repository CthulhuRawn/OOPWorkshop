$(document).ready(function(){
                        $('.dropdown-menu a').on('click', function(){ 
                                $('.dropdown-toggle').html($(this).html() + '  <span class="caret"></span>');    
                        })

                         $('.nav-tabs a').on('click', function (e) {
                                e.preventDefault();
                                $(this).tab('show');
                        })
                })