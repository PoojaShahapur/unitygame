#include "CenterTabWidget.h"
#include "ui_CenterTabWidget.h"

CenterTabWidget::CenterTabWidget(QWidget *parent)
	: QTabWidget(parent), m_ui(new Ui::CenterTabWidget)
{
	m_ui->setupUi(this);
}

CenterTabWidget::~CenterTabWidget()
{

}